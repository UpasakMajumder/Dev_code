// @Fflow
import React from 'react';
import PropTypes from 'prop-types';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { connect } from 'react-redux';

import { HAS_OVERLAY } from 'app.consts';
import BodyClassToggler from 'app.dump/BodyClassToggler';

type Props = {
  isShownHeaderShadow: string
};

const HeaderShadow = ({ isShownHeaderShadow }: Props) => (
  <BodyClassToggler
    className={HAS_OVERLAY}
    isActive={isShownHeaderShadow}
  >
    <ReactCSSTransitionGroup
      transitionName="header-overlay"
      transitionEnterTimeout={400}
      transitionLeaveTimeout={400}
      component="div"
    >
      {isShownHeaderShadow ? <div className="header-overlay" /> : null}
    </ReactCSSTransitionGroup>
  </BodyClassToggler>
);

HeaderShadow.propTypes = { isShownHeaderShadow: PropTypes.bool.isRequired };

export default connect(({ isShownHeaderShadow }) => ({ isShownHeaderShadow }), {})(HeaderShadow);
