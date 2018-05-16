import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* ac */
import toggleThumbnailPreview from 'app.ac/thumbnailPreview';

class ThumbnailPreview extends Component {
  static propTypes = {
    show: PropTypes.bool.isRequired,
    image: PropTypes.string,
    toggleThumbnailPreview: PropTypes.func.isRequired
  };

  handleHideOnEsc = (e) => {
    if (e.keyCode !== 27) return;
    this.props.toggleThumbnailPreview('');
  }

  componentDidUpdate() {
    if (this.props.show) {
      document.addEventListener('keyup', this.handleHideOnEsc);
    } else {
      document.removeEventListener('keyup', this.handleHideOnEsc);
    }
  }

  render() {
    const {
      show,
      image,
      toggleThumbnailPreview
    } = this.props;

    if (!show) return null;

    return (
      <div className="thumbnail-preview">
        <img className="thumbnail-preview__image" src={image} alt="" />
        <div onClick={() => toggleThumbnailPreview('')} className="thumbnail-preview__background"/>
      </div>
    );
  }
}

export default connect(({ thumbnailPreview }) => {
  return { ...thumbnailPreview };
}, {
  toggleThumbnailPreview
})(ThumbnailPreview);

