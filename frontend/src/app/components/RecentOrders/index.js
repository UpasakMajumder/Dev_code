import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Alert from 'app.dump/Alert';
import Pagination from 'app.dump/Pagination';
import Order from './Order';
import { getHeadings, getRows } from '../../AC/recentOrders';

class RecentOrders extends Component {
  state = {
    currPage: 0,
    prevPage: 0
  };

  static propTypes = {
    getHeadings: PropTypes.func.isRequired,
    getRows: PropTypes.func.isRequired,
    headings: PropTypes.arrayOf(PropTypes.string).isRequired,
    noOrdersMessage: PropTypes.string.isRequired,
    pageInfo: PropTypes.object.isRequired,
    rows: PropTypes.object.isRequired
  };

  changePage = ({ selected }) => {
    this.setState(({ currPage }) => {
      return {
        currPage: selected,
        prevPage: currPage
      };
    });
  };

  componentWillUpdate(nextProps, nextState) {
    const { getRows } = this.props;

    if (nextState.currPage === this.state.currPage) return;
    if (!Object.keys(nextProps.rows).length) return;
    if (nextProps.rows[nextState.currPage]) return;
    getRows(nextState.currPage + 1);
  }

  componentDidMount() {
    const { getHeadings, getRows } = this.props;
    const { currPage } = this.state;

    getHeadings();
    getRows(currPage + 1);
  }

  render() {
    const { headings, pageInfo, rows, noOrdersMessage } = this.props;
    const { pagesCount, rowsCount, rowsOnPage } = pageInfo;
    const { currPage, prevPage } = this.state;

    const headersList = headings.map((heading, index) => <th key={index}>{heading}</th>);
    const tableHeader = <tr>{headersList}</tr>;

    let tableRows = null;

    if (Object.keys(rows).length) {
      if (rows[currPage]) {
        tableRows = rows[currPage].map(row => <Order key={row.orderNumber} {...row}/>);
      } else {
        tableRows = rows[prevPage].map(row => <Order key={row.orderNumber} {...row}/>);
      }
    }

    let content = null;

    if (Object.keys(rows).length) {
      if (!rows[0].length) {
        content = <Alert type="info" text={noOrdersMessage} />;
      } else {
        content = (
          <div>
            <table className="show-table">
              <tbody>
              {tableHeader}
              {tableRows}
              </tbody>
            </table>

            <Pagination pagesNumber={pagesCount}
                        initialPage={0}
                        currPage={currPage}
                        itemsOnPage={rowsOnPage}
                        itemsNumber={rowsCount}
                        onPageChange={this.changePage} />
          </div>
        );
      }
    }

    return content;
  }
}

export default connect((state) => {
  const { recentOrders } = state;
  return { ...recentOrders };
}, {
  getHeadings,
  getRows
})(RecentOrders);
