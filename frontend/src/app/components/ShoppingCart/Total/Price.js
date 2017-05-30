import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Price extends Component {
  render() {
    const { title, value, className } = this.props;

    return (
      <div className={className}>
        <span className="summary-table__info">{title}</span>
        <span className="summary-table__amount">{value}</span>
      </div>
    );
  }
}

export default Price;
