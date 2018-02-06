import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import moment from 'moment';
import SVG from 'app.dump/SVG';
/* components */
import Spinner from 'app.dump/Spinner';
/* ac */
import { loadManageProducts } from 'app.ac/manageProducts';
/* helpers */
import timeFormat from 'app.helpers/time';

class ManageProducts extends Component {
  static propTypes = {
    isLoading: PropTypes.bool.isRequired,
    title: PropTypes.string.isRequired,
    loadManageProducts: PropTypes.func.isRequired,
    tableHeaders: PropTypes.array.isRequired,
    templates: PropTypes.array.isRequired
  };

  constructor() {
    super();

    this.state = {
      sortBy: '',
      sortOrderAsc: false,
      sortedTemplates: []
    };
  }

  sortByColumn = (name) => {
    const sortedTemplates = [...this.state.sortedTemplates].sort((template1, template2) => {
      if (template1[name] === null) {
        return Number.NEGATIVE_INFINITY;
      }
      if (template2[name] === null) {
        return Number.POSITIVE_INFINITY;
      }

      const name1 = template1[name].toUpperCase();
      const name2 = template2[name].toUpperCase();

      if (this.state.sortOrderAsc) {
        if (name1 < name2) return 1;
        return -1;
      }

      if (name1 > name2) return 1;
      return -1;
    });

    this.setState({
      sortBy: name,
      sortOrderAsc: !this.state.sortOrderAsc,
      sortedTemplates
    });
  };

  componentWillReceiveProps(nextProps) {
    // set initial sort values from tableHeaders
    nextProps.tableHeaders.find((column) => {
      const { defaultSort } = column;

      if (defaultSort === 'asc' || defaultSort === 'desc') {
        this.setState({
          sortBy: column.name,
          sortOrderAsc: defaultSort === 'asc'
        });
        return true;
      }
      return false;
    });

    this.setState({
      sortedTemplates: [...nextProps.templates]
    });
  }

  componentDidMount() {
    // load Templates
    this.props.loadManageProducts();
  }

  getHeaders = () => {
    const { sortBy, sortOrderAsc } = this.state;

    return this.props.tableHeaders.map((column) => {
      const { sortable } = column;
      const sorting = sortable
        ? (
          <span>
            <SVG style={{ transform: 'rotate(180deg)', opacity: sortOrderAsc && sortBy === column.name ? 1 : 0.2 }}
                name="arrowTop"
                className="icon-modal"
            />
            <SVG style={{ opacity: !sortOrderAsc && sortBy === column.name ? 1 : 0.2 }}
                name="arrowTop"
                className="icon-modal"
            />
          </span>
        ) : null;

      return (
        <th
          key={column.name}
          onClick={sortable ? () => this.sortByColumn(column.name) : null}
          style={{ cursor: sortable ? 'pointer' : 'initial' }}
        >
          {sorting}
          {column.title}
        </th>
      );
    });
  };

  getTemplates = () => {
    return this.state.sortedTemplates.map((template) => {
      return (
        <tr key={template.templateId} className="product-list__row js-redirection" data-url="#">
          <td className="product-list__item">
            <div className="product-list__preview" style={{ backgroundImage: `url(${template.image})` }} />
          </td>
          <td className="product-list__item">
            <a className="link weight--normal" href={template.editorUrl}>{template.productName}</a>
          </td>
          <td className="product-list__item">
            {template.createdDate && timeFormat(template.createdDate)}
          </td>
          <td className="product-list__item">
            {template.updatedDate && timeFormat(template.updatedDate)}
          </td>
          <td className="product-list__item">
            <div className="product-list__btn-group">
              <a href={template.editorUrl} className="btn-action product-list__btn--primary">{this.props.openInDesignBtn}</a>
            </div>
          </td>
        </tr>
      );
    });
  };

  render() {
    const { isLoading, title } = this.props;

    return (
      <div className="product-template__block">
        <div className="product-template__item">
          <h2 className="block__heading pt-4 pb-2">{title}</h2>
        </div>
        <div className="product-template__item">

          {isLoading && <Spinner />}

          {!isLoading && <table className="table table--opposite table--inside-border table--hover product-list">
            <tbody>

            <tr>
              {this.getHeaders()}
              <th></th>
            </tr>

            {this.getTemplates()}
            </tbody>
          </table>}
        </div>

        {/* TODO Pagination component */}
      </div>
    );
  }
}

export default connect((state) => {
  const { isLoading, tableHeaders, templates, title, openInDesignBtn } = state.manageProducts;
  return { isLoading, tableHeaders, templates, title, openInDesignBtn };
}, {
  loadManageProducts
})(ManageProducts);
