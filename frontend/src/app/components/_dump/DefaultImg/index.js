import React from 'react';
import PropTypes from 'prop-types';
import Img from 'app.gfx/images/default-img.jpg';

const DefaultImg = ({ img = Img, alt = 'default', className = ''}) => {
  return <img src={img} alt={alt} className={className} />;
};

DefaultImg.propTypes = {
  img: PropTypes.string,
  alt: PropTypes.string,
  className: PropTypes.string
};

export default DefaultImg;
