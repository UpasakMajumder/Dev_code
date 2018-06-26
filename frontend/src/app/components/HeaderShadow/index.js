import React from 'react';
import PropTypes from 'prop-types';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { connect } from 'react-redux';

const HeaderShadow = ({ isShownHeaderShadow }) => (
  <ReactCSSTransitionGroup
    transitionName="header-overlay"
    transitionEnterTimeout={400}
    transitionLeaveTimeout={400}
    component="div"
  >
    {isShownHeaderShadow ? <div className="header-overlay" /> : null}
  </ReactCSSTransitionGroup>
);

HeaderShadow.propTypes = { isShownHeaderShadow: PropTypes.bool.isRequired };

export default connect(({ isShownHeaderShadow }) => ({ isShownHeaderShadow }), {})(HeaderShadow);
