import React from 'react';
import PropTypes from 'prop-types';

function Page(props) {
  const { url, title } = props;
  return <a href ={url}>{title}</a>;
}

Page.propTypes = {
  url: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired
};

export default Page;
