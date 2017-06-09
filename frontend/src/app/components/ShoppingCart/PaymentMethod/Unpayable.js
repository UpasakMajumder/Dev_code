import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Unpayable extends Component {
  render() {
    const { unPayableText } = this.props;

    return (
      <div className="alert--grey alert--full alert--smaller isOpen">
        <span>{unPayableText}</span>
      </div>
    );
  }
}

export default Unpayable;
