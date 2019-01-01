import React from "react";
import Button from "../../components/Button";
import Card from "../../components/Card";
import { getAddressString, IFlatInfo } from "../../services/FlatService";
import Styles from "./FlatItem.module.css";

interface IFlatItemProps {
  flat: IFlatInfo;
  onClick: (id: string) => void;
}

const FlatItem = ({ flat, onClick }: IFlatItemProps) => {
  console.log(flat);
  const func = () => onClick(flat.id);
  return (
    <Card className={Styles.flat}>
      <div className={Styles.text}>
        <span className={Styles.title}>{flat.name}</span>
        <span className={Styles.address}>{getAddressString(flat.address)}</span>
      </div>
      <Button onClick={func} className={Styles.openButton}>
        Plačiau
      </Button>
    </Card>
  );
};

export default FlatItem;
