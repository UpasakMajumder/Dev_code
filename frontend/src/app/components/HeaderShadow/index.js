import React from 'react';
import { connect } from 'react-redux';

const HeaderShadow = ({ isShownHeaderShadow }) => {
  return isShownHeaderShadow ? <div className="header-overlay"> </div> : null;
};

export default connect(({ isShownHeaderShadow }) => ({ isShownHeaderShadow }), {})(HeaderShadow);
