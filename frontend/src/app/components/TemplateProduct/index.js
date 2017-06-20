import React, { Component } from 'react';
import PropTypes from 'prop-types';
import SVG from '../SVG';

class TemplateProduct extends Component {
  constructor() {
    super();

    this.state = {
      templateQuantity: 1
    };

    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(value) {
    this.setState({
      templateQuantity: value
    });
  }

  static getStockBlock(stockType, stockText) {
    let stockClass;

    switch (stockType) {
    case 'available':
      stockClass = 'stock--available';
      break;
    case 'unavailable':
      stockClass = 'stock--unavailable';
      break;
    case 'out':
    default:
      stockClass = 'stock--out';
      break;
    }

    return (
      <div className={`stock ${stockClass}`}>
        <SVG name={stockClass} className={`icon-stock icon-${stockClass}`}/>
        {stockText}
      </div>
    );
  }

  render() {
    const { templateQuantity } = this.state;
    const {
      isFavourite,
      imgUrl,
      breadcrumbs,
      title,
      stock,
      useTemplateBtn
    } = this.props;

    const { type: stockType, text: stockText } = stock;
    const { url: btnUrl, text: btnText } = useTemplateBtn;

    const breadcrumbsList = breadcrumbs.map((item, i) => <span key={i}>{item}</span>);

    return (
      <div className="template">
        <div className="template__img" style={{ backgroundImage: `url(${imgUrl})` }}> </div>

        <div className="template__breadcrumbs">
          {breadcrumbsList}
        </div>

        <div className="template__title">
          <h3 title={title}>{title}</h3>
        </div>

        {TemplateProduct.getStockBlock(stockType, stockText)}

        <div className="template__use">
          <input type="number"
                 className="input__text template__input"
                 value={templateQuantity}
                 onChange={(e) => { this.handleChange(e.target.value); }}
                 min="1" />
          <a href={btnUrl} className="btn-action">{btnText}</a>
        </div>
      </div>
    );
  }
}

TemplateProduct.propTypes = {
  isFavourite: PropTypes.bool,
  imgUrl: PropTypes.string,
  breadcrumbs: PropTypes.array,
  title: PropTypes.string.isRequired,
  stock: PropTypes.shape({ type: PropTypes.string.isRequired, text: PropTypes.string.isRequired }),
  useTemplateBtn: PropTypes.shape({ url: PropTypes.string.isRequired, text: PropTypes.string.isRequired })
};

export default TemplateProduct;
