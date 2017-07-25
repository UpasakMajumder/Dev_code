import React, { Component } from 'react';
import { connect } from 'react-redux';
import Spinner from '../Spinner';

class GlobalSpinner extends Component {
  render() {
    const { isLoading } = this.props;

    const spinner = <div className="spinner__wrapper"><Spinner /></div>;

    return isLoading ? spinner : null;
  }
}

export default connect((state) => {
  const { isLoading } = state;
  return { isLoading };
}, {})(GlobalSpinner);
