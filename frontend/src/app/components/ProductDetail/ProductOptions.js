import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
import uuid from 'uuid';
/* components */
import { Tooltip } from 'react-tippy';
import Checkbox from 'app.dump/Form/CheckboxInput';

const ProductOptions = ({
  productOptions,
  handleChangeOptions,
  stateOptions,
  optionsPrice,
  optionsError
}) => {
  if (!productOptions) return null;
  const categories = productOptions.get('categories');
  if (!categories || !categories.count()) return null;

  const getContent = () => {
    return categories.map((category) => {
      if (category.get('selector') === 'Dropdownlist') {
        const options = category.get('options').map((option) => {
          return (
            <option
              key={uuid()}
              disabled={option.get('disabled')}
              value={option.get('id')}
            >
              {option.get('name')}
            </option>
          );
        }).toJS();

        return (
          <div key={uuid()} className="input__wrapper product-options__input">
            <div className="input__select">
              <select
                value={stateOptions.get(category.get('name')) || 0}
                onChange={e => handleChangeOptions(category.get('name'), e.target.value)}
              >
                {options}
              </select>
            </div>
          </div>
        );
      } else if (category.get('selector') === 'RadioButtonsHorizontal' || category.get('selector') === 'RadioButtonsVertical') {
        const type = category.get('selector') === 'RadioButtonsHorizontal' ? 'row' : 'column';
        const options = category.get('options').map((option) => {
          const checked = stateOptions.get(category.get('name')) === option.get('id').toString() || option.get('selected');

          return (
            <Checkbox
              key={uuid()}
              id={option.get('id')}
              label={option.get('name')}
              type="radio"
              name={category.get('name')}
              disabled={option.get('disabled')}
              value={option.get('id')}
              checked={checked}
              onChange={e => handleChangeOptions(category.get('name'), e.target.value)}
            />
          );
        }).toJS();

        return (
          <div key={uuid()} className={`product-options__radio product-options__radio--${type}`}>
            {options}
          </div>
        );
      }

      return null;
    }).toJS();
  };

  const align = categories.findEntry(category => category.get('selector') === 'RadioButtonsVertical') ? 'top' : 'center';

  return (
      <Tooltip
        title={productOptions.get('validationMessage')}
        position="left"
        animation="perspective"
        open={optionsError}
        theme="danger"
        arrow
        className={`product-options product-options--row product-options--${align}`}
      >
        {getContent()}
      </Tooltip>
  );
};

ProductOptions.propTypes = {
  handleChangeOptions: PropTypes.func.isRequired,
  stateOptions: ImmutablePropTypes.map,
  optionsError: PropTypes.bool.isRequired,
  productOptions: ImmutablePropTypes.mapContains({
    validationMessage: PropTypes.string.isRequired,
    categories: ImmutablePropTypes.listOf(ImmutablePropTypes.mapContains({
      name: PropTypes.string.isRequired,
      selector: PropTypes.oneOf(['Dropdownlist', 'RadioButtonsHorizontal', 'RadioButtonsVertical']).isRequired,
      options: ImmutablePropTypes.listOf(ImmutablePropTypes.mapContains({
        name: PropTypes.string.isRequired,
        disabled: PropTypes.bool,
        selected: PropTypes.bool.isRequired,
        id: PropTypes.oneOfType([PropTypes.number, PropTypes.string])
      }).isRequired).isRequired
    }))
  })
};

export default ProductOptions;
