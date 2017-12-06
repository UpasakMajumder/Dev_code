import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Button from 'app.dump/Button';
import SVG from 'app.dump/SVG';
import TextInput from 'app.dump/Form/TextInput';
/* 3rd part components */
import { Tooltip } from 'react-tippy';

const EmailConfirmation = ({
  tooltipText,
  maxItems,
  items,
  fields,
  addInput,
  removeInput,
  changeInput,
  title,
  description
}) => {
  const getInputs = () => {
    return items.map((item, i) => {
      const addButton = i === items.length - 1 && i !== maxItems - 1
        ? (
          <Tooltip
            title={tooltipText.add}
            animation="fade"
            position="right"
            theme="dark"
            arrow
          >
            <Button
              type="common"
              btnClass="plus-btn email-confirmation__button"
              onClick={addInput}
            >
              <SVG className="icon-modal" name="plus" />
            </Button>
          </Tooltip>
        ) : null;

      const removeButton = i !== items.length - 1
        ? (
          <Tooltip
            title={tooltipText.remove}
            animation="fade"
            position="right"
            theme="dark"
            arrow
          >
            <Button
              type="common"
              btnClass="minus-btn email-confirmation__button"
              onClick={(e) => { removeInput(item.id); }}
            >
              <SVG className="icon-modal" name="minus" />
            </Button>
          </Tooltip>
        ) : null;

      return (
        <div className="email-confirmation__block" key={item.id}>
          <div className="email-confirmation__input">
            <TextInput
              value={fields[item.id]}
              onChange={(e) => { changeInput(item.id, e.target.value); }}
            />
          </div>
          {addButton}
          {removeButton}
        </div>
      );
    });
  };

  const descriptionElement = description
    ? <p className="cart-fill__info">{description}</p>
    : null;

  return (
    <div className="shopping-cart__block">
      <h2>{title}</h2>
      {descriptionElement}
      <div className="email-confirmation">
        {getInputs()}
      </div>
    </div>
  );
};

EmailConfirmation.propTypes = {
  maxItems: PropTypes.number,
  tooltipText: PropTypes.shape({
    add: PropTypes.string.isRequired,
    remove: PropTypes.string.isRequired
  }).isRequired,
  title: PropTypes.string.isRequired,
  description: PropTypes.string,
  items: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.number.isRequired
  })).isRequired,
  fields: PropTypes.object.isRequired,
  addInput: PropTypes.func.isRequired,
  removeInput: PropTypes.func.isRequired,
  changeInput: PropTypes.func.isRequired
};

export default EmailConfirmation;
