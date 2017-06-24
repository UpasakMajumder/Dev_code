import React, { Component } from 'react';
import { connect } from 'react-redux';
import Order from './Order';
import Pagination from '../Pagination';
import Alert from '../Alert';
import { getHeadings, getRows } from '../../AC/recentOrders';

class RecentOrders extends Component {
  constructor() {
    super();

    this.state = {
      currPage: 0,
      prevPage: 0
    };

    this.changePage = this.changePage.bind(this);
  }

  changePage({ selected }) {
    this.setState((prevState) => {
      return {
        currPage: selected,
        prevPage: prevState.currPage
      };
    });

  }

  componentWillUpdate(nextProps, nextState) {
    if (nextState.currPage === this.state.currPage) return;
    if (!Object.keys(nextProps.rows).length) return;
    if (nextProps.rows[nextState.currPage]) return;
    this.props.getRows(nextState.currPage + 1);
  }

  componentDidMount() {
    const { currPage } = this.state;
    this.props.getHeadings();
    this.props.getRows(currPage + 1);
  }

  render() {
    const { headings, pageInfo, rows, noOrdersMessage } = this.props;
    const { pagesCount, rowsCount, rowsOnPages } = pageInfo;
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
                        itemsOnPage={rowsOnPages}
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
