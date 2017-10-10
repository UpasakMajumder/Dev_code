import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import Alert from 'app.dump/Alert';
import Pagination from 'app.dump/Pagination';
import Spinner from 'app.dump/Spinner';
/* ac */
import { changePage, initUI } from 'app.ac/recentOrders';
/* local components */
import Order from './Order';

class RecentOrders extends Component {
  state = {
    currPage: 0,
    prevPage: 0
  };

  static propTypes = {
    changePage: PropTypes.func.isRequired,
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
    if (nextState.currPage === this.state.currPage) return;
    if (!Object.keys(nextProps.rows).length) return;
    if (nextProps.rows[nextState.currPage]) return;
    this.props.changePage(nextState.currPage + 1, true);
  }

  componentDidMount() {
    this.props.initUI();
  }

  render() {
    const { headings, pageInfo, rows, noOrdersMessage } = this.props;
    const { pagesCount, rowsCount, rowsOnPage } = pageInfo;
    const { currPage, prevPage } = this.state;

    const headersList = headings.map((heading, index) => <th key={index}>{heading}</th>);
    const tableHeader = <tr>{headersList}</tr>;

    let content = <Spinner />;

    if (Object.keys(rows).length) {
      if (!rows[0].length) {
        content = <Alert type="info" text={noOrdersMessage} />;
      } else {
        let tableRows = null;

        if (rows[currPage]) {
          tableRows = rows[currPage].map(row => <Order key={row.orderNumber} {...row}/>);
        } else {
          tableRows = rows[prevPage].map(row => <Order key={row.orderNumber} {...row}/>);
        }

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
  changePage,
  initUI
})(RecentOrders);
