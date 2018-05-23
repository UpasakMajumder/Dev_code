import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Tabs extends Component {
  static defaultProps = {
    tabs: [
      {
        id: 0,
        text: 'Orders requiring approval',
        url: '#1'
      },
      {
        id: 1,
        text: 'Orders',
        url: '#1'
      }
    ],
    content: [
      {
        id: 0,
        content: () => <div>Hi</div>
      },
      {
        id: 1,
        content: () => <div>Hello</div>
      }
    ]
  };

  static propTypes = {
    tabs: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
      text: PropTypes.string.isRequired
    }).isRequired).isRequired,
    content: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
      content: PropTypes.func.isRequired // React component is a function
    }).isRequired).isRequired
  }

  constructor(props) {
    super(props);

    const { tabs } = props;

    this.state = {
      activeTabId: tabs ? tabs[0].id : 0
    };
  }

  handleChangeTab = activeTabId => this.setState({ activeTabId });

  render() {
    const tabs = this.props.tabs.map((tab) => {
      return (
        <li
          key={tab.id}
          className={`css-tabs__tab js-tab-recent-orders ${this.state.activeTabId === tab.id ? 'active' : ''}`}
          onClick={() => this.handleChangeTab(tab.id)}
        >
          {tab.text}
        </li>
      );
    });

    const ActiveElement = this.props.content.find(element => element.id === this.state.activeTabId).content;
    const acriveElementProps = this.props.tabs.find(tab => tab.id === this.state.activeTabId);

    return (
      <div className="css-tabs__container js-tabs">
        <ul className="css-tabs__list">
          {tabs}
        </ul>
        <div>
          <div className="css-tabs__content active show">
            <ActiveElement {...acriveElementProps} />
          </div>
        </div>
      </div>
    );
  }
}

export default Tabs;
