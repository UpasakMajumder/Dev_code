import React from 'react';
import PropTypes from 'prop-types';
import Img from 'app.gfx/images/default-img.jpg';

const DefaultImg = ({ img, alt, className }) => {
  return <img src={img || Img} alt={alt || 'default'} className={className || ''} />;
};

DefaultImg.propTypes = {
  img: PropTypes.string,
  alt: PropTypes.string,
  className: PropTypes.string
};

export default DefaultImg;
