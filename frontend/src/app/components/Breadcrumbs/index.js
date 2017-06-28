import React from 'react';
import PropTypes from 'prop-types';

const Breadcrumbs = ({ crumbs }) => {
  const items = crumbs.map(crumb => <a key={crumb.title} href={crumb.link}>{crumb.title}</a>);
  return <div className="breadcrumbs">{items}</div>;
};

Breadcrumbs.propTypes = {
  crumbs: PropTypes.arrayOf(PropTypes.string.isRequired).isRequired
};

export default Breadcrumbs;
