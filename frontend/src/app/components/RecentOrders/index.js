import React, { Component } from 'react';
import { connect } from 'react-redux';
import Order from './Order';
import Spinner from '../Spinner';
import { getHeadings, getRows } from '../../AC/recentOrders';

class Table extends Component {
  constructor() {
    super();

    this.state = {
      page: 0
    };
  }

  componentDidMount() {
    const { page } = this.state;
    this.props.getHeadings();
    this.props.getRows(page + 1);
  }

  render() {
    const { page } = this.state;
    const { headings, pagination, rows } = this.props;

    const headersList = headings.map((heading, index) => <th key={index}>{heading}</th>);
    const tableHeader = <tr>{headersList}</tr>;

    const tableRows = Object.keys(rows).length
      ? rows[page].map(row => <Order key={row.orderNumber} {...row}/>)
      : null;


    const content = headings.length
    ? (
      <table className="show-table">
        <tbody>
        {tableHeader}
        {tableRows}
        </tbody>
      </table>
      )
    : <Spinner />;

    return content;
  }
}

export default connect((state) => {
  const { recentOrders } = state;
  return { ...recentOrders };
}, {
  getHeadings,
  getRows
})(Table);
