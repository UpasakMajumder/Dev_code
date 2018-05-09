import React, { Component } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
/* constants */
import { FAILURE, APP_LOADING, START, FINISH } from 'app.consts';
/* globals */
import { RECENT_ORDERS } from 'app.globals';
/* components */
import Alert from 'app.dump/Alert';
import Pagination from 'app.dump/Pagination';
import Spinner from 'app.dump/Spinner';
/* local components */
import Order from './Order';

class RecentOrders extends Component {
  state = {
    currPage: 0,
    prevPage: 0,
    headings: [],
    pageInfo: {},
    rows: {},
    noOrdersMessage: ''
  };

  static propTypes = {
    initURL: PropTypes.string.isRequired
  };

  changePage = ({ selected }) => {
    if (selected === this.state.currPage) return;
    axios.get(`${RECENT_ORDERS.getPageItems}/${selected + 1}`)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;
        window.store.dispatch({ type: APP_LOADING + START });

        if (!success) {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        } else {
          this.setState(({ currPage, rows }) => {
            return {
              rows: {
                ...rows,
                [selected]: payload.rows
              },
              currPage: selected,
              prevPage: currPage
            };
          });
        }
        window.store.dispatch({ type: APP_LOADING + FINISH });
      })
      .catch((error) => {
        window.store.dispatch({
          type: FAILURE,
          alert: false
        });
        window.store.dispatch({ type: APP_LOADING + FINISH });
      });
  };

  initUI = (url) => {
    axios.get(url)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        } else {
          this.setState({
            headings: payload.headings,
            pageInfo: payload.pageInfo,
            noOrdersMessage: payload.noOrdersMessage,
            rows: {
              ...this.state.rows,
              0: payload.rows
            }
          });
        }
      })
      .catch(() => {
        window.store.dispatch({
          type: FAILURE,
          alert: false
        });
      });
  };

  componentDidMount() {
    this.initUI(this.props.initURL);
  }

  getPaginationComponent = () => {
    const { pageInfo, currPage } = this.state;

    if (!pageInfo) return null;

    const { pagesCount, rowsCount, rowsOnPage } = pageInfo;

    return (
      <Pagination
        pagesNumber={pagesCount}
        initialPage={0}
        currPage={currPage}
        itemsOnPage={rowsOnPage}
        itemsNumber={rowsCount}
        onPageChange={this.changePage}
      />
    );
  }

  getTableHeader = () => {
    const headersList = this.state.headings.map((heading, index) => <th key={index}>{heading}</th>);
    return (
      <tr>
        {headersList}
      </tr>
    );
  };

  getTableRows = () => {
    const {
      rows,
      currPage,
      prevPage
    } = this.state;

    const list = rows[currPage] || rows[prevPage];
    const tableRows = list.map(row => <Order key={row.orderNumber} {...row}/>);

    return tableRows;
  };

  render() {
    const {
      rows,
      noOrdersMessage
    } = this.state;

    let content = <Spinner />;

    if (Object.keys(rows).length) {
      if (!rows[0].length) {
        content = <Alert type="info" text={noOrdersMessage} />;
      } else {


        content = (
          <div>
            <table className="show-table">
              <tbody>
              {this.getTableHeader()}
              {this.getTableRows()}
              </tbody>
            </table>

            {this.getPaginationComponent()}
          </div>
        );
      }
    }

    return content;
  }
}

export default RecentOrders;
