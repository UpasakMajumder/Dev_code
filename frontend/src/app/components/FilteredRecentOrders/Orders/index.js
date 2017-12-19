import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* helpers */
import timeFormat from 'app.helpers/time';
/* components */
import Dialog from '../Dialog';

class Orders extends Component {
  static propTypes = {
    orders: PropTypes.shape({
      headers: PropTypes.arrayOf(PropTypes.string.isRequired),
      rows: PropTypes.arrayOf(PropTypes.shape({
        dialog: PropTypes.object.isRequired,
        items: PropTypes.arrayOf(PropTypes.shape({
          value: PropTypes.string.isRequired,
          type: PropTypes.string,
          url: PropTypes.string
        })).isRequired
      }))
    }).isRequired
  }

  state = {
    activeDialog: ''
  }

  closeDialog = () => {
    this.setState({ activeDialog: '' });
  };

  setActiveDialog = (activeDialog) => {
    this.setState({ activeDialog });
  };

  getDialog = () => {
    const { activeDialog } = this.state;
    if (activeDialog === '') return null;
    const { dialog } = this.props.orders.rows[activeDialog];
    return <Dialog dialog={dialog} closeDialog={this.closeDialog} />;
  };

  getTableHeader = () => {
    const headersList = this.props.orders.headers.map((header, i) => <th key={i}>{header}</th>);
    return <tr>{headersList}</tr>;
  };

  getTableRows = () => {
    return this.props.orders.rows.map((row, i) => {
      const cells = row.items.map((item, j) => {
        let cell = <td key={j}>{item.value}</td>;

        if (item.type === 'link') {
          cell = <td key={j}><a target="_blank" href={item.url} className="link">{item.value}</a></td>;
        } else if (item.type === 'button') {
          cell = (
            <td key={j} className="show-table__will-appear">
              <button
                type="button"
                className="btn-action"
                onClick={() => { this.setActiveDialog(i); }}
              >
                {item.value}
              </button>
            </td>
          );
        } else if (!isNaN(Date.parse(item.value)) && isNaN(Date.UTC(item.value))) {
          cell = <td key={j}>{timeFormat(item.value)}</td>;
        }

        return cell;
      });

      return <tr key={i}>{cells}</tr>;
    });
  };

  render() {
    return (
      <div>
        {this.getDialog()}
        <table className="show-table">
          <tbody>
            {this.getTableHeader()}
            {this.getTableRows()}
          </tbody>
        </table>
      </div>
    );
  }
}

export default Orders;
