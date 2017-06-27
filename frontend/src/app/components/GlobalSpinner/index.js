import React, { Component } from 'react';
import { connect } from 'react-redux';
import Spinner from '../Spinner';

const GlobalSpinner = ({ isLoading }) => {
  return isLoading ? <div className="spinner__wrapper"><Spinner /></div> : null;
};

export default connect(({ isLoading }) => ({ isLoading }), {})(GlobalSpinner);
