import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

const HeaderShadow = ({ isShownHeaderShadow }) => {
  return isShownHeaderShadow ? <div className="header-overlay"> </div> : null;
};

HeaderShadow.propTypes = {
  isShownHeaderShadow: PropTypes.bool.isRequired
};

export default connect(({ isShownHeaderShadow }) => ({ isShownHeaderShadow }), {})(HeaderShadow);
