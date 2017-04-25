import React, { PropTypes } from 'react';

const SVG = (props) => {
  const { name, className } = props;
  const Icon = require(`../../../gfx/svg/${name}.svg`); // eslint-disable-line global-require

  return <Icon.default className={`icon ${className || ''}`} />;
};

SVG.propTypes = {
  name: PropTypes.string.isRequired,
  className: PropTypes.string
};

export default SVG;
