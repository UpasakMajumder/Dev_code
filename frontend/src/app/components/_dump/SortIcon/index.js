import React from 'react';
import PropTypes from 'prop-types';
import SVG from 'app.dump/SVG';

const SortIcon = ({
  block,
  wrapperClass,
  sortOrderAsc,
  isActive
}) => {
  const svgs = [1, 2].map((svg, index) => {
    const style = {
      opacity: !sortOrderAsc && isActive ? 1 : 0.2
    };

    if (index > 0) {
      style.transform = 'rotate(180deg)';
      style.opacity = sortOrderAsc && isActive ? 1 : 0.2;
    }

    return (
      <SVG
        key={index}
        style={style}
        name="arrowTop"
        className="icon-modal"
      />
    );
  });

  if (block) return <div className={wrapperClass}>{svgs}</div>;
  return <span className={wrapperClass}>{svgs}</span>;
};

SortIcon.defaultProps = {
  block: true,
  wrapperClass: ''
};

SortIcon.propTypes = {
  block: PropTypes.bool,
  wrapperClass: PropTypes.string,
  sortOrderAsc: PropTypes.bool.isRequired,
  isActive: PropTypes.bool.isRequired
};

export default SortIcon;
