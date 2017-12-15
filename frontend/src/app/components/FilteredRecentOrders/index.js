import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* globals */
import { FILTERED_RECENT_ORDERS } from 'app.globals';
/* AC */
import { getCampaigns, getOrders } from 'app.ac/filteredRecentOrders';
/* components */
import Select from 'app.dump/Form/Select';
import Spinner from 'app.dump/Spinner';
import Orders from './Orders';

class FilteredRecentOrders extends Component {
  methodsToggler = (value) => {
    const valueObj = FILTERED_RECENT_ORDERS.orderTypes.items.filter(item => item.id === value)[0];
    valueObj.campaigns ? this.getCampaigns(value) : this.getOrders(null, value);
  }

  getOrders = (selectedCampaign, selectedOrderType) => {
    const url = FILTERED_RECENT_ORDERS.getOrdersUrl;
    this.props.getOrders(url, selectedOrderType || this.props.filteredRecentOrders.orderType, selectedCampaign);
  };

  getCampaigns = (selectedOrderType) => {
    const url = FILTERED_RECENT_ORDERS.getCampaignsUrl;
    this.props.getCampaigns(url, selectedOrderType);
  };

  getCampaignElement = () => {
    const { campaign, isFetching } = this.props.filteredRecentOrders;
    if (!campaign.items.length) return null;
    return (
      <Select
        disabled={isFetching}
        options={campaign.items}
        onChange={(e) => { this.getOrders(e.target.value); }}
        value={campaign.value}
        placeholder={campaign.placeholder}
      />
    );
  };

  getOrdersElement = () => {
    const { orders, isFetching } = this.props.filteredRecentOrders;
    if (isFetching) return <Spinner />;
    if (!Object.keys(orders).length) return null;
    return <Orders orders={orders} />;
  };

  render() {
    const { orderType, isFetching } = this.props.filteredRecentOrders;

    return (
      <div>
        <Select
          disabled={isFetching}
          options={FILTERED_RECENT_ORDERS.orderTypes.items}
          onChange={(e) => { this.methodsToggler(e.target.value); }}
          value={orderType}
          placeholder={FILTERED_RECENT_ORDERS.orderTypes.placeholder}
        />
        {this.getCampaignElement()}
        {this.getOrdersElement()}
      </div>
    );
  }
}

FilteredRecentOrders.propTypes = {
  getCampaigns: PropTypes.func.isRequired,
  getOrders: PropTypes.func.isRequired,
  filteredRecentOrders: PropTypes.shape({
    isFetching: PropTypes.bool.isRequired,
    orderType: PropTypes.string,
    campaign: PropTypes.shape({
      value: PropTypes.string,
      placeholder: PropTypes.string,
      items: PropTypes.array.isRequired
    }).isRequired,
    orders: PropTypes.object.isRequired
  }).isRequired
};

export default connect((state) => {
  const { filteredRecentOrders } = state;
  return { filteredRecentOrders };
}, {
  getCampaigns,
  getOrders
})(FilteredRecentOrders);
