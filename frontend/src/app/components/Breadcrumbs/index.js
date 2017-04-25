import React from 'react';
import PropTypes from 'prop-types';

function Breadcrumbs(props) {
  const { crumbs } = props;

  const items = crumbs.map((crumb) => {
    const { title, link } = crumb;
    return <a key={title} href={link}>{title}</a>;
  });

  return (
    <div className="breadcrumbs">
      { items }
    </div>
  );
}

Breadcrumbs.propTypes = {
  crumbs: PropTypes.array.isRequired
};

export default Breadcrumbs;
