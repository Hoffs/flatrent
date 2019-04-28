import React, { ReactNode } from "react";
import { TextRowLoader } from "../../../../components/Loaders";
import { IFlatDetails } from "../../../../services/interfaces/FlatServiceInterfaces";
import { dayOrDays, joined } from "../../../../utilities/Utilities";
import Styles from "./FlatDetails.module.css";

const FlatDescription = ({ flat }: { flat?: IFlatDetails }) => {
  const rentPeriod = (days: number): ReactNode => (
    <>
      Trumpiausias nuomos laikotarpis {days} {dayOrDays(days)}.
    </>
  );

  return (
    <>
      <span className={Styles.sectionTitle}>Aprašymas</span>
      <span className={joined(Styles.flatDescription, Styles.sectionEnd)}>
        {flat === undefined ? <TextRowLoader rows={6} width={600} /> : flat.description}
      </span>

      <span className={Styles.sectionTitle}>Pageidavimai nuomininkui</span>
      <span className={Styles.tenantRequirements}>
        {flat === undefined ? <TextRowLoader rows={6} width={600} /> : flat.tenantRequirements}
      </span>
      <span className={joined(Styles.minimumRentDays, Styles.sectionEnd)}>
        {flat === undefined ? <TextRowLoader rows={1} width={600} /> : rentPeriod(flat.minimumRentDays)}
      </span>
    </>
  );
};

export default FlatDescription;
