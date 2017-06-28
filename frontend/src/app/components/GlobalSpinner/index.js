import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Spinner from '../Spinner';

const GlobalSpinner = ({ isLoading }) => {
  return isLoading ? <div className="spinner__wrapper"><Spinner /></div> : null;
};

GlobalSpinner.propTypes = {
  isLoading: PropTypes.bool.isRequired
};

export default connect(({ isLoading }) => ({ isLoading }), {})(GlobalSpinner);
