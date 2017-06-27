import React from 'react';
import PropTypes from 'prop-types';

const Page = ({ url, title }) => <a href={url}>{title}</a>;

Page.propTypes = {
  url: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired
};

export default Page;
