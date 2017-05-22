import axios from 'axios';
import { CARD_VALIDATION_ERROR, SUBMIT_CARD } from '../constants';
import { cardPaymentSymbols } from '../helpers/validationRules';
import { CARD_PAYMENT } from '../globals';

export default function submitCard(fields, cardType) {
  const { name, cvc, number, expiry } = fields;

  return (dispatch) => {
    const maxLength = cardType === 'amex' ? cardPaymentSymbols.number.amex : cardPaymentSymbols.number.rest;
    if (number.length < maxLength) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'number',
          errorMessage: CARD_PAYMENT.fields.number.inValidMessage
        }
      });
      return;
    }

    if (!CARD_PAYMENT.acceptedCards.includes(cardType)) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'number',
          errorMessage: CARD_PAYMENT.cardTypeInValidMessage
        }
      });
      return;
    }

    if (name.length < cardPaymentSymbols.name.min) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'name',
          errorMessage: CARD_PAYMENT.fields.name.inValidMessage
        }
      });
      return;
    }

    if (cvc.length < cardPaymentSymbols.cvc.min) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'cvc',
          errorMessage: CARD_PAYMENT.fields.cvc.inValidMessage
        }
      });
      return;
    }

    if (expiry.length < cardPaymentSymbols.expiry.min) {
      dispatch({
        type: CARD_VALIDATION_ERROR,
        payload: {
          errorField: 'expiry',
          errorMessage: CARD_PAYMENT.fields.expiry.inValidMessage
        }
      });
      return;
    }

    dispatch({
      type: SUBMIT_CARD
    });

    // AJAX REQUEST

    const { URL3DSi, ResultURL, BrowserRedirectURL, SubmissionID,
            CustomerIdentifier_MerchantCode,
            CustomerIdentifier_LocationCode,
            CustomerIdentifier_CustomerCode,
            TerminalIdentifier_LocationCode,
            TerminalIdentifier_TerminalCode,
            TerminalIdentifier_MerchantCode
    } = CARD_PAYMENT;

    const data = {
      CreditCard_CSCValue: cvc,
      CreditCard_ExpirationMonth: expiry.substr(0, 2),
      CreditCard_ExpirationYear: expiry.substr(2),
      CreditCard_CardType: cardType,
      CreditCard_NameOnCard: name,
      CreditCard_CardAccountNumber: '', // -
      APCount: '', // -
      PTCount: '', // -
      ResultURL,
      BrowserRedirectURL,
      SubmissionID,
      CustomerIdentifier_MerchantCode,
      CustomerIdentifier_LocationCode,
      CustomerIdentifier_CustomerCode,
      TerminalIdentifier_LocationCode,
      TerminalIdentifier_TerminalCode,
      TerminalIdentifier_MerchantCode
    };

    axios({
      method: 'post',
      url: URL3DSi,
      headers: { 'Content-type': 'application/x-www-form-urlencoded' },
      data
    });
  };
}
