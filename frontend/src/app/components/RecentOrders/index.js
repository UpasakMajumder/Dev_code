import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import moment from 'moment';
/* components */
import Alert from 'app.dump/Alert';
import Pagination from 'app.dump/Pagination';
import Spinner from 'app.dump/Spinner';
import SortIcon from 'app.dump/SortIcon';
/* ac */
import { getRows, changeDate } from 'app.ac/recentOrders';
/* globals */
import { RECENT_ORDERS } from 'app.globals';
/* helpers */
import { sortObjs } from 'app.helpers/array';
import { createSearchStr } from 'app.helpers/location';
/* local components */
import Order from './Order';
import DateFilter from './DateFilter';

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
          show: PropTypes.bool.isRequired
        }).isRequired
      }).isRequired,
      getRowsUrl: PropTypes.string.isRequired,
      noOrdersMessage: PropTypes.string.isRequired
    }).isRequired,
    getRows: PropTypes.func.isRequired,
    changeDate: PropTypes.func.isRequired,
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
        sortBy: PropTypes.string
      }).isRequired,
      filter: PropTypes.shape({
        orderDate: PropTypes.object.isRequired
      }).isRequired
    }).isRequired
  }

  static getSortId(sortOrderAsc, sortBy) {
    if (typeof sortBy === 'undefined') return '';

    const sortOrder = sortOrderAsc ? 'ASC' : 'DESC';
    const sortId = `${sortBy}-${sortOrder}`;

    return sortId;
  }

  static generateUrl(defaultUrl, args = {}) {
    let url = `${defaultUrl}/${args.page}`;
    if (typeof args.page !== 'undefined') return url;

    const search = {};

    if (args.sort) search.sort = args.sort;
    if (args.dateFrom) search.dateFrom = args.dateFrom;
    if (args.dateTo) search.dateTo = args.dateTo;
    if (args.export) search.format = 'xlsx';

    const searchUrl = Object.keys(search).length ? createSearchStr(search) : '';

    url = `${defaultUrl}${searchUrl}`;

    return url;
  }

  static getFormattedDate(date) {
    return date !== null ? moment(date).format('YYYY-MM-DD') : '';
  }

  componentDidMount() {
    this.props.getRows(RecentOrders.generateUrl(this.props.pageInfo.getRowsUrl));
  }

  handleChangePage = (page) => {
    if (page === this.props.store.pagination.currentPage) return;
    const url = RecentOrders.generateUrl(this.props.pageInfo.getRowsUrl, { page });
    this.props.getRows(url, { page });
  };

  getHeadings = () => {
    const headings = this.props.headings.map((heading, index) => {
      const onClick = heading.sort ? () => this.sortColumn(heading.id) : null;

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
          <span className="ml-2">{heading.label}</span>
        </th>
      );
    });
    return <tr>{headings}</tr>;
  };

  getRows = () => {
    const rowElements = this.props.store.rows.map((row, index) => <Order headings={this.props.headings} key={index} {...row}/>);
    return rowElements;
  };

  sortColumn = (sortBy) => {
    const newSortOrderAsc = !this.props.store.sort.sortOrderAsc;
    const sort = RecentOrders.getSortId(newSortOrderAsc, sortBy);

    const { dateFrom, dateTo } = this.props.store.filter.orderDate;

    const args = {
      sort,
      sortOrderAsc: newSortOrderAsc,
      sortBy,
      dateFrom: RecentOrders.getFormattedDate(dateFrom),
      dateTo: RecentOrders.getFormattedDate(dateTo)
    };

    const url = RecentOrders.generateUrl(this.props.pageInfo.getRowsUrl, args);

    this.props.getRows(url, args);
  };

  getContent = () => {
    let content = <Spinner />;

    if (this.props.store.rows.length) {
      const { pagination } = this.props.store;

      content = (
        <div>
          <div className="overflow--y-scroll">
            <table className="show-table show-table--pointer">
              <tbody>
                {this.getHeadings()}
                {this.getRows()}
              </tbody>
            </table>
          </div>

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

  changeDate = (value, field) => {
    this.props.changeDate(value, field);
  };

  applyDate = () => {
    const { dateFrom, dateTo } = this.props.store.filter.orderDate;
    if (!dateFrom) return;

    const sort = RecentOrders.getSortId(this.props.store.sort.sortOrderAsc, this.props.store.sort.sortBy);

    const args = {
      sort,
      dateFrom: RecentOrders.getFormattedDate(dateFrom),
      dateTo: RecentOrders.getFormattedDate(dateTo)
    };

    const url = RecentOrders.generateUrl(this.props.pageInfo.getRowsUrl, args);

    this.props.getRows(url, args);
  };

  getDateFilter = () => {
    const { orderDate } = this.props.pageInfo.filter;

    if (!orderDate.show) return null;
    return (
      <DateFilter
        ui={orderDate}
        dateFrom={this.props.store.filter.orderDate.dateFrom}
        dateTo={this.props.store.filter.orderDate.dateTo}
        changeDate={this.changeDate}
        applyDate={this.applyDate}
      />
    );
  };

  getExportLink = () => {
    const args = {
      export: true,
      sort: RecentOrders.getSortId(this.props.store.sort.sortOrderAsc, this.props.store.sort.sortBy),
      dateFrom: RecentOrders.getFormattedDate(this.props.store.filter.orderDate.dateFrom),
      dateTo: RecentOrders.getFormattedDate(this.props.store.filter.orderDate.dateTo)
    };

    const url = RecentOrders.generateUrl(this.props.pageInfo.export.url, args);
    const link = <a className="btn-action" href={url}>{this.props.pageInfo.export.label}</a>;
    return link;
  };

  render() {
    return (
      <div>
        <div className="flex--end--between mb-3">
          {this.getDateFilter()}
          {this.getExportLink()}
        </div>

        {this.getContent()}
      </div>
    );
  }
}

RecentOrders.defaultProps = { ...RECENT_ORDERS };

export default connect((state) => {
  const { recentOrders } = state;
  return { store: { ...recentOrders } };
}, {
  getRows,
  changeDate
})(RecentOrders);
