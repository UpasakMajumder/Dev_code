import React, { Component } from 'react';
import PropTypes from 'prop-types';
import MethodsGroup from './MethodsGroup';

class DeliveryMethod extends Component {
  constructor() {
    super();

    this.state = {
      openId: 0
    };

    this.changeOpenId = this.changeOpenId.bind(this);
  }

  componentDidMount() {
    const { items } = this.props.ui;
    const openedMethodGroup = items.filter((item) => { return item.opened; })[0];
    this.setState({
      openId: openedMethodGroup.id
    });
  }

  changeOpenId(openId) {
    this.setState({
      openId
    });
  }

  render() {
    const { openId } = this.state;
    const { ui, checkedId, changeShoppingData } = this.props;
    const { title, description, items } = ui;

    const methodsGroups = items.map((item) => {
      return (
        <MethodsGroup
          openId={openId}
          changeOpenId={this.changeOpenId}
          changeShoppingData={changeShoppingData}
          checkedId={checkedId}
          key={`mg-${item.id}`}
          {...item} />
      );
    });

    const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

    return (
      <div>
        <h2>{title}</h2>
        <div className="cart-fill__block">
          {descriptionElement}
          <div className="cart-fill__block-inner">
            { methodsGroups }
          </div>
        </div>
      </div>
    );
  }
}

export default DeliveryMethod;
