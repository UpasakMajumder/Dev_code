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
import Tabs from 'app.dump/Tabs';
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
    orders: null,
    activeTabId: '',
    orderTypeId: '',
    campaignFilter: {
      value: '',
      items: [],
      placeholder: ''
    },
    isDisabled: false,
    isOpacity: false,
    isFetching: false
  }

  handleChangeTab = (activeTabId) => {
    this.setState({
      activeTabId,
      isDisabled: true,
      isOpacity: true
    }, () => {
      let url = this.props.filterItems[this.state.orderTypeId].tabs[activeTabId].getOrdersUrl;
      if (this.state.campaignFilter.value) url += `/${this.state.campaignFilter.value}`;
      this.fetchOrders(url);
    });
  };

  fetchOrders = (url) => {
    axios
      .get(url)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          this.setState({
            orders: payload,
            isDisabled: false,
            isOpacity: false,
            isFetching: false
          });
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((e) => {
        window.store.dispatch({ type: FAILURE });
      });
  }

  getOrdersElement = () => {
    if (this.state.isFetching) return <Spinner />;
    if (!this.state.orders) return null;

    const tabList = this.props.filterItems[this.state.orderTypeId].tabs;
    let tabs = null;

    if (tabList.length > 1) {
      tabs = tabList.map((tab, id) => {
        return {
          id,
          text: tab.text,
          onClick: () => this.handleChangeTab(id)
        };
      });
    }

    return (
      <Tabs
        tabs={tabs}
        activeTabId={this.state.activeTabId}
      >
        <Orders isOpacity={this.state.isOpacity} orders={this.state.orders} />
      </Tabs>
    );
  };

  getCampaignElement = () => {
    const { campaignFilter, isDisabled } = this.state;
    if (!campaignFilter.items.length) return null;
    return (
      <Select
        disabled={isDisabled}
        options={campaignFilter.items}
        onChange={this.handleChangeCampaign}
        value={campaignFilter.value}
        placeholder={campaignFilter.placeholder}
      />
    );
  };

  getCampaigns = (url, orderTypeId) => {
    axios
      .get(url)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (success) {
          this.setState({
            campaignFilter: {
              ...this.setState.campaignFilter,
              placeholder: payload.placeholder,
              items: payload.items
            },
            isDisabled: false
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
    const campaignValue = event.target.value;
    this.setState({
      campaignFilter: {
        ...this.state.campaignFilter,
        value: campaignValue
      },
      activeTabId: 0,
      isDisabled: true,
      isFetching: true
    }, () => {
      this.fetchOrders(`${this.props.filterItems[this.state.orderTypeId].tabs[0].getOrdersUrl}/${campaignValue}`);
    });
  };

  handleChangeOrderType = (event) => {
    const orderTypeId = event.target.value;

    const activeFilter = this.props.filterItems[orderTypeId];
    const { getCampaignsUrl } = activeFilter;

    if (getCampaignsUrl) {
      this.setState({
        orderTypeId,
        orders: null,
        activeTabId: '',
        campaignFilter: {
          value: '',
          items: [],
          placeholder: ''
        },
        isDisabled: true
      });
      this.getCampaigns(getCampaignsUrl);
    } else {
      this.setState({
        orderTypeId,
        activeTabId: 0,
        orders: null,
        campaignFilter: {
          value: '',
          items: [],
          placeholder: ''
        },
        isDisabled: true,
        isFetching: true
      }, () => {
        this.fetchOrders(this.props.filterItems[orderTypeId].tabs[0].getOrdersUrl);
      });
    }
  };

  render() {
    return (
      <div>
        <div className="filtered-recent-orders">
          <div className="filtered-recent-orders__input">
            <Select
              disabled={this.state.isDisabled}
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
