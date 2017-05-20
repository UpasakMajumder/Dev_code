import { CARD_VALIDATION_ERROR, SUBMIT_CARD } from '../constants';

export default function submitCard(fields, cardType) {
  const { name, cvc, number, expiry } = fields;

  return (dispatch) => {
    const maxLength = cardType === 'amex' ? 15 : 16;
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

    if (!['visa', 'amex', 'mastercard'].includes(cardType)) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'number',
          errorMessage: 'type error'
        }
      });
      return;
    }

    if (name.length < 1) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'name',
          errorMessage: 'name error'
        }
      });
      return;
    }

    if (cvc.length < 3) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'cvc',
          errorMessage: 'cvc error'
        }
      });
      return;
    }

    if (expiry.length < 4) {
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
