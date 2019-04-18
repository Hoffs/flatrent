import React, { ChangeEvent } from "react";

const rent = () => null;

export default rent;

// private getRentCard = (flat: IFlatDetails) => {
//   const today = new Date();
//   const fromExtraProps = {
//     max: getOffsetDate(1, 0, 7),
//     min: getOffsetDate(0, 0, 0),
//   };
//   const toExtraProps = {
//     max: getOffsetDate(1, 0, 7),
//     min: getOffsetDate(0, 1, 7),
//   };

//   if (this.state.values.from !== undefined && this.state.values.from.length > 0) {
//     const split = this.state.values.from.split("-");
//     const date = new Date();
//     toExtraProps.min = getOffsetDate(
//       Number(split[0]) - date.getFullYear(),
//       Number(split[1]) - date.getMonth(),
//       Number(split[2]) - date.getDate()
//     );
//   }

//   return !this.state.showRentCard ? (
//     <></>
//   ) : (
//     <Card key="rentCard" className={Styles.rentCard}>
//       <span className={Styles.title}>Nuomos sutartis</span>
//       <FlexRow className={Styles.shortRow}>
//         <span className={Styles.name}>Kaina per mėnesį:</span>
//         <span className={Styles.value}>{flat.price} Eur</span>
//       </FlexRow>
//       <InputForm errors={this.state.errors.General} errorsOnly={true} name="" title="" setValue={doNothing} />
//       <FlexRow className={Styles.pickerRow}>
//         <MuiThemeProvider theme={materialTheme}>
//           <MuiPickersUtilsProvider utils={DateFnsUtils} locale={locale}>
//             <FlexColumn>
//               <InlineDatePicker
//                 label="Nuo"
//                 disablePast={true}
//                 minDate={fromExtraProps.min}
//                 minDateMessage="Per trumpas laikotarpis"
//                 maxDate={fromExtraProps.max}
//                 maxDateMessage="Per ilgas laikotarpis"
//                 format="yyyy-MM-dd"
//                 value={this.state.values.from}
//                 onChange={this.handleDateFromChange}
//                 variant="outlined"
//               />
//               <InputForm errorsOnly={true} errors={this.state.errors.From} name="" title="" setValue={doNothing} />
//             </FlexColumn>
//             <FlexColumn>
//               <InlineDatePicker
//                 label="Iki"
//                 format="yyyy-MM-dd"
//                 disablePast={true}
//                 minDate={toExtraProps.min}
//                 minDateMessage="Per trumpas laikotarpis"
//                 maxDate={toExtraProps.max}
//                 maxDateMessage="Per ilgas laikotarpis"
//                 value={this.state.values.to}
//                 onChange={this.handleDateToChange}
//                 variant="outlined"
//               />
//               <InputForm errorsOnly={true} errors={this.state.errors.To} name="" title="" setValue={doNothing} />
//             </FlexColumn>
//           </MuiPickersUtilsProvider>
//         </MuiThemeProvider>
//       </FlexRow>
//       <FlexRow>
//         <InputArea
//           className={Styles.textArea}
//           errors={this.state.errors.from}
//           name="comments"
//           title="Komentarai"
//           setValue={this.handleDataUpdate}
//         />
//       </FlexRow>
//       <FlexRow className={Styles.buttonRow}>
//         <Button outline={true} onClick={this.handleTitleButton} className={Styles.buttonCancel}>
//           Atšaukti
//         </Button>
//         <Button disabled={this.state.requesting} outline={true} onClick={this.handleSubmit}>
//           Pateikti
//         </Button>
//       </FlexRow>
//     </Card>
//   );
// };
