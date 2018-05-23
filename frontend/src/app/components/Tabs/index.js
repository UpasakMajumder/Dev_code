import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Tabs extends Component {
  static propTypes = {
    tabs: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
      text: PropTypes.string.isRequired
    }).isRequired).isRequired
  }

  constructor(props) {
    super(props);

    const { tabs } = props;

    this.state = {
      activeTabId: tabs ? tabs[0].id : 0
    };
  }

  handleChangeTab = (activeTabId, tabFn) => {
    this.setState({ activeTabId });
    tabFn && tabFn();
  };

  render() {
    const tabs = this.props.tabs.map((tab) => {
      return (
        <li
          key={tab.id}
          className={`css-tabs__tab js-tab-recent-orders ${this.state.activeTabId === tab.id ? 'active' : ''}`}
          onClick={() => this.handleChangeTab(tab.id, tab.tabFn)}
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
