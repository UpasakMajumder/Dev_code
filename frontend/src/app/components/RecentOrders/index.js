import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import Alert from 'app.dump/Alert';
import Pagination from 'app.dump/Pagination';
import Spinner from 'app.dump/Spinner';
/* ac */
import { changePage, getRows } from 'app.ac/recentOrders';
/* globals */
import { RECENT_ORDERS } from 'app.globals';
/* local components */
import Order from './Order';

class RecentOrders extends Component {
  static propTypes = {
    headings: PropTypes.arrayOf(PropTypes.shape({
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
      }).isRequired
    }).isRequired
  }

  // static propTypes = {
  //   changePage: PropTypes.func.isRequired,
  //   headings: PropTypes.arrayOf(PropTypes.string).isRequired,
  //   noOrdersMessage: PropTypes.string.isRequired,
  //   pageInfo: PropTypes.object.isRequired,
  //   rows: PropTypes.object.isRequired
  // };

  // changePage = ({ selected }) => {
  //   this.setState(({ currPage }) => {
  //     return {
  //       currPage: selected,
  //       prevPage: currPage
  //     };
  //   });
  // };

  // componentWillUpdate(nextProps, nextState) {
  //   if (nextState.currPage === this.state.currPage) return;
  //   if (!Object.keys(nextProps.rows).length) return;
  //   if (nextProps.rows[nextState.currPage]) return;
  //   this.props.changePage(nextState.currPage + 1, true);
  // }

  componentDidMount() {
    this.props.getRows(this.props.pageInfo.getRowsUrl);
  }

  handleChangePage = (page) => {
    if (page === this.props.store.pagination.currentPage) return;
    this.props.getRows(this.props.pageInfo.getRowsUrl, { page });
  };

  getHeadings = () => {
    const headings = this.props.headings.map((heading, index) => <th key={index}>{heading.label}</th>);
    return <tr>{headings}</tr>;
  };

  getRows = () => {
    const rows = this.props.store.rows.map((row, index) => <Order headings={this.props.headings} key={index} {...row}/>);
    return rows;
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

  //   const { headings, pageInfo, rows, noOrdersMessage } = this.props;
  //   const { pagesCount, rowsCount, rowsOnPage } = pageInfo;
  //   const { currPage, prevPage } = this.state;

  //   const headersList = headings.map((heading, index) => <th key={index}>{heading}</th>);
  //   const tableHeader = <tr>{headersList}</tr>;

  //   let content = <Spinner />;

  //   if (Object.keys(rows).length) {
  //     if (!rows[0].length) {
  //       content = <Alert type="info" text={noOrdersMessage} />;
  //     } else {
  //       let tableRows = null;

  //       if (rows[currPage]) {
  //         tableRows = rows[currPage].map(row => <Order key={row.orderNumber} {...row}/>);
  //       } else {
  //         tableRows = rows[prevPage].map(row => <Order key={row.orderNumber} {...row}/>);
  //       }

  //       content = (
  //         <div>
  //           <table className="show-table">
  //             <tbody>
  //             {tableHeader}
  //             {tableRows}
  //             </tbody>
  //           </table>
  //         </div>
  //       );
  //     }
  //   }

  //   return content;
  // }
}

RecentOrders.defaultProps = { ...RECENT_ORDERS };

export default connect((state) => {
  const { recentOrders } = state;
  return { store: { ...recentOrders } };
}, {
  getRows
})(RecentOrders);
