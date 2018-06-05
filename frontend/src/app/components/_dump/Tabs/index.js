import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Tabs extends Component {
  static propTypes = {
    tabs: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      text: PropTypes.string.isRequired,
      onClick: PropTypes.func.isRequired
    }).isRequired),
    activeTabId: PropTypes.oneOfType([PropTypes.number, PropTypes.string])
  }

  render() {
    const tabs = this.props.tabs && this.props.tabs.map((tab) => {
      return (
        <li
          key={tab.id}
          className={`css-tabs__tab js-tab-recent-orders ${this.props.activeTabId === tab.id ? 'active' : ''}`}
          onClick={tab.onClick}
        >
          {tab.text}
        </li>
      );
    });

    return (
      <div className="css-tabs__container js-tabs">
        <ul className="css-tabs__list">
          {tabs}
        </ul>
        <div>
          <div className="css-tabs__content active show">
            {this.props.children}
          </div>
        </div>
      </div>
    );
  }
}

export default Tabs;
