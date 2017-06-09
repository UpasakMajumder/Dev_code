import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Address extends Component {
  render() {
    const { unDeliverableText } = this.props;

    return (
      <div className="alert--grey alert--full alert--smaller isOpen">
        <span>{unDeliverableText}</span>
      </div>
    );
  }
}

export default Address;
