import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Tooltip from 'react-tooltip-component';
import SVG from '../SVG';

class Steps extends Component {
  render() {
    const { steps, current } = this.props;
    const width = `${100 / steps.length}%`;

    const items = steps.map((step, index) => {
      let road = {
        className: '',
        icon: null
      };

      if (index < current) {
        road = {
          className: 'previous',
          icon: <SVG name="tick" />
        };
      } else if (index === current) {
        road = {
          className: 'current',
          icon: <SVG name="gear" />
        };
      }

      return (
        <div key={index} className="steps__item" style={{ width }}>
          <Tooltip title={step} position="right">
            <div className={`steps__circle ${road.className}`}>
              { road.icon }
            </div>
          </Tooltip>
        </div>

      );
    });
    return (
      <div className="steps">
        { items }
        <div className="steps__line"></div>
      </div>
    );
  }
}

Steps.propTypes = {
  steps: PropTypes.array,
  current: PropTypes.number
};

export default Steps;
