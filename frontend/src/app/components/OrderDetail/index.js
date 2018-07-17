// @flow
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import axios from 'axios';
/* components */
import Spinner from 'app.dump/Spinner';
/* utilities */
import { getSearchObj } from 'app.helpers/location';
/* globals */
import { ORDER_DETAIL as ORDER_DETAIL_URL } from 'app.globals';
/* constants */
import { FAILURE } from 'app.consts';
/* local components */
import CommonInfo from './CommonInfo';
import ShippingInfo from './ShippingInfo';
import PaymentInfo from './PaymentInfo';
import PricingInfo from './PricingInfo';
import OrderedItems from './OrderedItems';
import Actions from './Actions';
import EditModal from './EditModal';
// import EmailProof from '../EmailProof';

type UI = {
  dateTimeNAString: string,
  commonInfo: {
    status: {
      value: string,
      note: string
    },
    totalCost: {
      value: string
    }
  },
  orderedItems: {
    items: Array<{ id: string, quantity: number, lineNumber: number }>
  },
  paymentInfo: {
    paymentIcon: string
  },
  pricingInfo: ?{
    items: []
  },
  shippingInfo: {},
  actions: {},
  general: {},
  editOrders: {
    proceedUrl: string,
    dialog: {}
  },
  emailProof: {
    show: boolean,
    url: string
  }
}

type State = {
  showEditModal: boolean,
  ui: ?UI,
  emailProof: {
    show: boolean,
    url: string
  }
}

class OrderDetail extends Component<void, void, State> {
  maxOrderQuantity: any;

  state = {
    showEditModal: false, // managed in Actions component
    ui: null,
    emailProof: {
      show: false,
      url: ''
    }
  }

  componentDidMount() {
    const orderID: string = getSearchObj().orderID || '';
    const url: string = `${ORDER_DETAIL_URL.orderDetailUrl}/${orderID}`;

    axios
      .get(url)
      .then((response: {
        data: {
          payload: UI,
          success: boolean,
          errorMessage: string
        }
      }): void => {
        const { payload, success, errorMessage } = response.data;
        if (!success) {
          window.sessionStorage.dispatch({ type: FAILURE, alert: errorMessage });
        } else {
          this.setState({ ui: payload });
        }
      })
      .catch((error): void => {
        window.sessionStorage.dispatch({ type: FAILURE });
      });
  }

  getMaxOrderQuantity = (): {} => {
    if (this.maxOrderQuantity || !this.state.ui) return this.maxOrderQuantity;

    const maxOrderQuantity = {};
    this.state.ui.orderedItems.items.forEach((orderedItem: { id: string, quantity: number }) => {
      maxOrderQuantity[orderedItem.id] = orderedItem.quantity;
    });

    this.maxOrderQuantity = maxOrderQuantity;

    return maxOrderQuantity;
  };

  changeStatus = (newStatus: string, note: string): void => {
    const { ui } = this.state;
    if (!ui) return;
    this.setState({
      ui: {
        ...ui,
        commonInfo: {
          ...ui.commonInfo,
          status: {
            ...ui.commonInfo.status,
            value: newStatus,
            note
          }
        }
      }
    });
  };

  showEditModal = (showEditModal: boolean) => this.setState({ showEditModal });

  toogleEmailProof = (url: string): void => {
    this.setState((prevState) => {
      return {
        emailProof: {
          show: !prevState.show,
          url
        }
      };
    });
  };

  editOrders = ({
    pricingInfo,
    orderedItems,
    ordersPrice
  }: {
    pricingInfo: [],
    orderedItems: [],
    ordersPrice: []
  }) => {
    const { ui } = this.state;
    if (!ui) return;

    this.setState({
      ui: {
        ...ui,
        commonInfo: {
          ...ui.commonInfo,
          totalCost: {
            ...ui.commonInfo.totalCost,
            value: pricingInfo.length ? pricingInfo[pricingInfo.length - 1].value : '' // totalCost is always the last item
          }
        },
        pricingInfo: pricingInfo.length ? {
          ...ui.pricingInfo,
          items: pricingInfo
        } : null,
        orderedItems: {
          ...ui.orderedItems,
          items: ui.orderedItems.items.map((item) => {
            const orderedItem = orderedItems.find(orderedItem => orderedItem.lineNumber === item.lineNumber);
            if (!orderedItem) return item;

            const priceItem = ordersPrice.find(order => order.lineNumber === item.lineNumber);

            return {
              ...item,
              removed: orderedItem.removed,
              quantity: orderedItem.quantity,
              price: priceItem && priceItem.price
            };
          })
        }
      }
    });
  };

  render() {
    const { ui, emailProof } = this.state;
    if (!ui) return <Spinner />;

    const {
      commonInfo,
      shippingInfo,
      paymentInfo,
      pricingInfo,
      orderedItems,
      dateTimeNAString,
      actions,
      general,
      editOrders
    } = ui;

    const shippingInfoEl = shippingInfo ? <div className="col-lg-4 mb-4"><ShippingInfo ui={shippingInfo} /></div> : null;
    const paymentInfoEl = paymentInfo ? <div className="col-lg-4 mb-4"><PaymentInfo ui={paymentInfo} dateTimeNAString={dateTimeNAString} /></div> : null;
    const pricingInfoEl = pricingInfo ? <div className="col-lg-4 mb-4"><PricingInfo ui={pricingInfo} /></div> : null;

    const editModal = editOrders
      ? (
        <EditModal
          closeModal={() => this.showEditModal(false)}
          open={this.state.showEditModal}
          orderedItems={orderedItems.items}
          {...editOrders.dialog}
          proceedUrl={this.state.ui ? this.state.ui.editOrders.proceedUrl : ''}
          paidByCreditCard={paymentInfo.paymentIcon === 'credit-card'}
          editOrders={this.editOrders}
          general={general}
          maxOrderQuantity={this.getMaxOrderQuantity()}
        />
      ) : null;

    const nonZeroProductsExist = Boolean(orderedItems.items.filter(item => item.quantity > 0).length);

    return (
      <div>
        {/* <EmailProof open={emailProof.show} /> */}
        {editModal}
        <CommonInfo
          ui={commonInfo}
          dateTimeNAString={dateTimeNAString}
        />

        <div className="order-block">
          <div className="row">
            {shippingInfoEl}
            {paymentInfoEl}
            {pricingInfoEl}
          </div>
        </div>

        {nonZeroProductsExist && (
          <OrderedItems
            toogleEmailProof={this.toogleEmailProof}
            ui={orderedItems}
            showRejectionLabel={false}
          />
        )}

        <div className="order-block">
          <Actions
            acceptEnabled={nonZeroProductsExist}
            actions={actions}
            editOrders={editOrders}
            editEnabled={nonZeroProductsExist}
            general={general}
            changeStatus={this.changeStatus}
            showEditModal={this.showEditModal}
          />
        </div>
      </div>
    );
  }
}

export default OrderDetail;
