import React from 'react';
import PropTypes from 'prop-types';
import SVG from 'app.dump/SVG';

const Method = (props) => {
  const { title, icon, disabled, id, hasInput, inputPlaceholder, checkedObj,
    changeShoppingData } = props;
  let { className } = props;

  const additionalInput = hasInput
    ? <div className="input__wrapper">
        <input onChange={(e) => { changeShoppingData(e.target.name, id, e.target.value); }}
               type="text"
               className="input__text"
               name="paymentMethod"
               placeholder={inputPlaceholder}
               value={checkedObj.invoice}
        />
      </div>
    : null;

  if (disabled) className += ' input__wrapper--disabled';

  return (
    <div className={className}>
      <input disabled={disabled}
             onChange={(e) => { changeShoppingData(e.target.name, id); }}
             checked={id === checkedObj.id}
             id={`pm-${id}`}
             name="paymentMethod"
             type="radio"
             className="input__radio" />
      <label htmlFor={`pm-${id}`} className="input__label input__label--radio">
        <SVG name={icon} className='icon-shipping' />
        <span>{title}</span>
      </label>
      {additionalInput}
    </div>
  );
};

Method.propTypes = {
  validationMessage: PropTypes.string.isRequired,
  changeShoppingData: PropTypes.func.isRequired,
  className: PropTypes.string.isRequired,
  checked: PropTypes.bool.isRequired,
  title: PropTypes.string.isRequired,
  icon: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired,
  inputPlaceholder: PropTypes.string,
  disabled: PropTypes.bool,
  hasInput: PropTypes.bool,
  checkedObj: PropTypes.shape({
    id: PropTypes.number.isRequired,
    invoice: PropTypes.string
  }).isRequired
};

export default Method;
