import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import Alert from 'app.dump/Alert';
import Pagination from 'app.dump/Pagination';
import Spinner from 'app.dump/Spinner';
import SortIcon from 'app.dump/SortIcon';
/* ac */
import { getRows, sortOrders } from 'app.ac/recentOrders';
/* globals */
import { RECENT_ORDERS } from 'app.globals';
/* helpers */
import { sortObjs } from 'app.helpers/array';
/* local components */
import Order from './Order';

class RecentOrders extends Component {
  static propTypes = {
    headings: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.string.isRequired,
      label: PropTypes.string.isRequired,
      sort: PropTypes.bool,
      isDate: PropTypes.bool
    }).isRequired).isRequired,
    pageInfo: PropTypes.shape({
      export: PropTypes.shape({
        label: PropTypes.string.isRequired,
        url: PropTypes.string.isRequired
      }).isRequired,
      filter: PropTypes.shape({
        orderDate: PropTypes.shape({
          title: PropTypes.string.isRequired,
          labelNow: PropTypes.string.isRequired
        }).isRequired
      }).isRequired,
      getRowsUrl: PropTypes.string.isRequired,
      noOrdersMessage: PropTypes.string.isRequired
    }).isRequired,
    getRows: PropTypes.func.isRequired,
    store: PropTypes.shape({
      rows: PropTypes.arrayOf(PropTypes.object).isRequired,
      pagination: PropTypes.shape({
        currentPage: PropTypes.number.isRequired,
        pagesCount: PropTypes.number,
        rowsCount: PropTypes.number,
        rowsOnPage: PropTypes.number
      }).isRequired,
      sort: PropTypes.shape({
        sortOrderAsc: PropTypes.bool.isRequired,
        sortBy: PropTypes.string.isRequired
      }).isRequired
    }).isRequired
  }

  componentDidMount() {
    this.props.getRows(this.props.pageInfo.getRowsUrl);
  }

  handleChangePage = (page) => {
    if (page === this.props.store.pagination.currentPage) return;
    this.props.getRows(this.props.pageInfo.getRowsUrl, { page });
  };

  getHeadings = () => {
    const headings = this.props.headings.map((heading, index) => {
      const onClick = heading.sort ? () => this.sortColumn(index, heading.id) : null;

      const sortIcon = heading.sort
        ? (
          <SortIcon
            block={false}
            sortOrderAsc={this.props.store.sort.sortOrderAsc}
            isActive={heading.id === this.props.store.sort.sortBy}
          />
        ) : null;

      return (
        <th
          key={heading.id}
          onClick={onClick}
          style={{ cursor: heading.sort ? 'pointer' : 'initial' }}
        >
          {sortIcon}
          {heading.label}
        </th>
      );
    });
    return <tr>{headings}</tr>;
  };

  getRows = () => {
    const rowElements = this.props.store.rows.map((row, index) => <Order headings={this.props.headings} key={index} {...row}/>);
    return rowElements;
  };

  sortColumn = (sortIndex, sortBy) => {
    const sortedRows = sortObjs(this.props.store.rows, sortIndex, this.props.store.sort.sortOrderAsc, 'items');
    const newSortOrderAsc = !this.props.store.sort.sortOrderAsc;

    this.props.sortOrders(sortedRows, newSortOrderAsc, sortBy, sortIndex);
  };

  getContent = () => {
    let content = <Spinner />;

    if (this.props.store.rows.length) {
      const { pagination } = this.props.store;

      content = (
        <div>
          <table className="show-table">
            <tbody>
              {this.getHeadings()}
              {this.getRows()}
            </tbody>
          </table>

          <Pagination
            pagesNumber={pagination.pagesCount}
            initialPage={pagination.currentPage}
            currPage={pagination.currentPage}
            itemsOnPage={pagination.rowsOnPage}
            itemsNumber={pagination.rowsCount}
            onPageChange={({ selected }) => { this.handleChangePage(selected); }}
          />
        </div>
      );
    }

    return content;
  };

  render() {
    return (
      <div>{this.getContent()}</div>
    );
  }
}

RecentOrders.defaultProps = { ...RECENT_ORDERS };

export default connect((state) => {
  const { recentOrders } = state;
  return { store: { ...recentOrders } };
}, {
  getRows,
  sortOrders
})(RecentOrders);
