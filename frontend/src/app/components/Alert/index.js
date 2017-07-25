import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Alert extends Component {
  render() {
    const { text, type } = this.props;

    return (
      <div className={`alert--${type} alert--full alert--smaller isOpen`}>
        <span>{text}</span>
      </div>
    );
  }
}

Alert.propTypes = {
  type: PropTypes.string.isRequired,
  text: PropTypes.string.isRequired
};

export default Alert;
