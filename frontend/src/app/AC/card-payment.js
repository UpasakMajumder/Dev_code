import axios from 'axios';
import { CARD_VALIDATION_ERROR, SUBMIT_CARD, GET_SUBMISSIONID, FETCH, FAILURE, SUCCESS } from 'app.consts';
import { cardPaymentSymbols } from 'app.helpers/validationRules';
import { CARD_PAYMENT, NOTIFICATION } from 'app.globals';

export default (fields, cardType) => {
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

    // AJAX REQUEST

    const submit = async () => {
      dispatch({ type: GET_SUBMISSIONID + FETCH });

      const { URL3DSi, ResultURL, RedirectURL, SubmissionIDURL,
        CustomerIdentifier_MerchantCode,
        CustomerIdentifier_LocationCode,
        CustomerIdentifier_CustomerCode,
        TerminalIdentifier_LocationCode,
        TerminalIdentifier_TerminalCode,
        TerminalIdentifier_MerchantCode,
        APCount, PTCount
      } = CARD_PAYMENT;

      const { data: { success, errorMessage, payload } } = await axios({ method: 'get', url: SubmissionIDURL });

      if (!success) {
        dispatch({
          type: GET_SUBMISSIONID + FAILURE,
          alert: errorMessage
        });
      } else {
        dispatch({ type: GET_SUBMISSIONID + SUCCESS });

        const { SubmissionID } = payload;
        const data = {
          CreditCard_CSCValue: cvc,
          CreditCard_ExpirationMonth: expiry.substr(0, 2),
          CreditCard_ExpirationYear: expiry.substr(2),
          CreditCard_CardType: cardType,
          CreditCard_NameOnCard: name,
          CreditCard_CardAccountNumber: number,
          APCount,
          PTCount,
          ResultURL,
          SubmissionID,
          CustomerIdentifier_MerchantCode,
          CustomerIdentifier_LocationCode,
          CustomerIdentifier_CustomerCode,
          TerminalIdentifier_LocationCode,
          TerminalIdentifier_TerminalCode,
          TerminalIdentifier_MerchantCode
        };

        dispatch({ type: SUBMIT_CARD + FETCH });

        axios({
          method: 'post',
          url: URL3DSi,
          headers: { 'Content-type': 'application/x-www-form-urlencoded' },
          data
        });

        axios({
          method: 'get',
          url: `${RedirectURL}/${SubmissionID}`
        }).then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({
              type: SUBMIT_CARD + FAILURE,
              alert: errorMessage
            });
          } else {
            dispatch({ type: SUBMIT_CARD + SUCCESS });
            location.assign(payload);
          }
        }).catch(() => {
          dispatch({ type: SUBMIT_CARD + FAILURE });
        });
      }
    };

    submit().catch(() => {
      dispatch({ type: SUBMIT_CARD + FAILURE });
    });
  };
};
