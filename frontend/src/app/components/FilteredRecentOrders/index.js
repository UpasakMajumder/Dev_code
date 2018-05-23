import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import uuid from 'uuid';
import axios from 'axios';
/* globals */
import { FILTERED_RECENT_ORDERS } from 'app.globals';
/* AC */
import { getCampaigns, getOrders } from 'app.ac/filteredRecentOrders';
/* consts */
import { FAILURE } from 'app.consts';
/* components */
import Tabs from 'app.smart/Tabs';
import Select from 'app.dump/Form/Select';
import Spinner from 'app.dump/Spinner';
import Orders from './Orders';

class FilteredRecentOrders extends Component {
  static defaultProps = { ...FILTERED_RECENT_ORDERS };

  static propTypes = {
    placeholder: PropTypes.string.isRequired,
    filterItems: PropTypes.arrayOf(PropTypes.shape({
      name: PropTypes.string.isRequired,
      getCampaignsUrl: PropTypes.string,
      tabs: PropTypes.arrayOf(PropTypes.shape({
        text: PropTypes.string.isRequired,
        getOrdersUrl: PropTypes.string.isRequired
      }).isRequired).isRequired
    }).isRequired).isRequired
  };

  state = {
    isFetching: false,
    orderTypeId: null,
    showContent: false,
    campaignFilter: {
      placeholder: '',
      items: [],
      value: ''
    },
    orders: {}
  }

  // methodsToggler = (value) => {
  //   const valueObj = FILTERED_RECENT_ORDERS.orderTypes.items.filter(item => item.id === value)[0];
  //   valueObj.campaigns ? this.getCampaigns(value) : this.getOrders(null, value);
  // }

  // getOrders = (selectedCampaign, selectedOrderType) => {
  //   const url = FILTERED_RECENT_ORDERS.getOrdersUrl;
  //   this.props.getOrders(url, selectedOrderType || this.props.filteredRecentOrders.orderType, selectedCampaign);
  // };

  // getCampaigns = (selectedOrderType) => {
  //   const url = FILTERED_RECENT_ORDERS.getCampaignsUrl;
  //   this.props.getCampaigns(url, selectedOrderType);
  // };

  // getOrdersElement = () => {
  //   const { orders, isFetching } = this.props.filteredRecentOrders;
  //   if (isFetching) return <Spinner />;
  //   if (!Object.keys(orders).length) return null;
  //   return <Orders orders={orders} />;
  // };

  fetchOrders = (url) => {
    axios
      .get(url)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          this.setState({
            orders: payload
          });
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch(() => {
        window.store.dispatch({ type: FAILURE });
      });
  }

  getOrdersElement = () => {
    if (!this.state.showContent) return null;

    const tabList = this.props.filterItems[this.state.orderTypeId].tabs;

    const tabs = tabList.map((tab, id) => {
      const url = `${tab.getOrdersUrl}/${this.state.campaignFilter.value}`;
      const tabFn = () => this.fetchOrders(url);
      if (!Object.keys(this.state.orders).length && id === 0) tabFn();
      return { ...tab, tabFn, id };
    });

    return (
      <Tabs
        tabs={tabs}
      >
        <Orders orders={this.state.orders} />
      </Tabs>
    );

  };

  getCampaignElement = () => {
    const { campaignFilter, isFetching } = this.state;
    if (!campaignFilter.items.length) return null;
    return (
      <Select
        disabled={isFetching}
        options={campaignFilter.items}
        onChange={this.handleChangeCampaign}
        value={campaignFilter.value}
        placeholder={campaignFilter.placeholder}
      />
    );
  };

  getCampaigns = (url) => {
    axios
      .get(url)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          this.setState({
            campaignFilter: {
              placeholder: payload.placeholder,
              items: payload.items
            }
          });
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch(() => {
        window.store.dispatch({ type: FAILURE });
      });
  };

  handleChangeCampaign = (event) => {
    this.setState({
      showContent: true,
      orders: {},
      campaignFilter: {
        ...this.state.campaignFilter,
        value: event.target.value
      }
    });
  };

  handleChangeOrderType = (event) => {
    const orderTypeId = event.target.value;

    const activeFilter = this.props.filterItems[orderTypeId];
    const { getCampaignsUrl } = activeFilter;

    this.setState({
      orderTypeId,
      campaignFilter: {
        placeholder: '',
        items: [],
        value: ''
      },
      showContent: !getCampaignsUrl,
      orders: {}
    }, () => {

      if (getCampaignsUrl) {
        this.getCampaigns(getCampaignsUrl);
      }
    });
  };

  render() {
    return (
      <div>
        <div className="filtered-recent-orders">
          <div className="filtered-recent-orders__input">
            <Select
              disabled={this.state.isFetching}
              options={this.props.filterItems.map((item, id) => ({ ...item, id }))}
              onChange={this.handleChangeOrderType}
              value={this.state.orderTypeId}
              placeholder={this.props.placeholder}
            />
          </div>
          <div className="ml-3 filtered-recent-orders__input">
            {this.getCampaignElement()}
          </div>
        </div>
        <div className="mt-3">
          {this.getOrdersElement()}
        </div>
      </div>
    );
  }
}

export default FilteredRecentOrders;
