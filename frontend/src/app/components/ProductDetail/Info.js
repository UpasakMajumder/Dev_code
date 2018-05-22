import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';

const Info = ({ info }) => {
  if (!info) return null;

  return (
    <div className="product-view__info">
      <div className="product-view__info-description">
        <span>{info.get('createdDate')}</span>
        <span>{info.get('code')}</span>
      </div>
    </div>
  );
};

Info.propTypes = {
  info: ImmutablePropTypes.mapContains({
    createdDate: PropTypes.string,
    code: PropTypes.string
  })
};

export default Info;
