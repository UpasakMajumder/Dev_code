import { CARD_VALIDATION_ERROR, SUBMIT_CARD } from '../constants';
import { cardPaymentSymbols } from '../helpers/validationRules';

export default function submitCard(fields, cardType) {
  const { name, cvc, number, expiry } = fields;

  return (dispatch) => {
    const maxLength = cardType === 'amex' ? cardPaymentSymbols.number.amex : cardPaymentSymbols.number.rest;
    if (number.length < maxLength) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'number',
          errorMessage: 'number error'
        }
      });
      return;
    }

    if (!['visa', 'amex', 'mastercard'].includes(cardType)) { // GLOBALS
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'number',
          errorMessage: 'type error'
        }
      });
      return;
    }

    if (name.length < cardPaymentSymbols.name.min) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'name',
          errorMessage: 'name error'
        }
      });
      return;
    }

    if (cvc.length < cardPaymentSymbols.cvc.min) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'cvc',
          errorMessage: 'cvc error'
        }
      });
      return;
    }

    if (expiry.length < cardPaymentSymbols.expiry.min) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'expiry',
          errorMessage: 'expiry error'
        }
      });
      return;
    }

    dispatch({
      type: SUBMIT_CARD
    });

    // AJAX REQUEST
  };
}
