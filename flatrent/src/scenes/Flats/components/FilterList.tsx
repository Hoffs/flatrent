import React, { ChangeEvent, useState } from "react";
import { RouteComponentProps } from "react-router-dom";
import FlexRow from "../../../components/FlexRow";
import { joined } from "../../../utilities/Utilities";
import Styles from "./FilterList.module.css";
import NumberFilterItem from "./NumberFilterItem";
import TextFilterItem from "./TextFilterItem";
import Button from "../../../components/Button";
import PerfectScrollbar from "react-perfect-scrollbar";
import "react-perfect-scrollbar/dist/css/styles.css";


interface IFlatFilterProps extends RouteComponentProps {
    onFilterUpdate: (filterString: string) => void;
    className?: string;
}

interface IFlatFilterState {
    [key: string]: string;
}

const FilterList = (props: IFlatFilterProps) => {
    const [queries, setQueries] = useState<IFlatFilterState>({ });

    const setQueriesFactory = (idx: string) => (val: string) => setQueries({ ...queries, [idx]: val });

    const getFullQueryString = Object.values(queries).join("&");
    const onSearch = () => {
        props.onFilterUpdate(getFullQueryString);
        props.history.push(`flats?${getFullQueryString}`);
    };
    return (
        <FlexRow className={joined(Styles.list, props.className)}>
            <NumberFilterItem
                onQueryUpdate={setQueriesFactory("1")}
                name="roomCountFrom"
                title="Kambarių skaičius nuo"
                {...props}
            />
            <NumberFilterItem
                onQueryUpdate={setQueriesFactory("2")}
                name="areaFrom"
                title="Plotas nuo"
                {...props}
            />
            <NumberFilterItem
                onQueryUpdate={setQueriesFactory("3")}
                name="floorFrom"
                title="Aukštas nuo"
                {...props}
            />
            <NumberFilterItem
                onQueryUpdate={setQueriesFactory("4")}
                name="floorTo"
                title="Aukštas iki"
                {...props}
            />
            <NumberFilterItem
                onQueryUpdate={setQueriesFactory("5")}
                name="priceFrom"
                title="Kaina nuo"
                {...props}
            />
            <NumberFilterItem
                onQueryUpdate={setQueriesFactory("6")}
                name="priceTo"
                title="Kaina iki"
                {...props}
            />
            <NumberFilterItem
                onQueryUpdate={setQueriesFactory("7")}
                name="rentDays"
                title="Nuomos laikotarpis dienomis"
                {...props}
            />

            <TextFilterItem onQueryUpdate={setQueriesFactory("8")} name="city" title="Miestas" {...props} />
            <Button className={Styles.searchButton} onClick={onSearch}>Ieškoti</Button>
        </FlexRow>
    );
};

export default FilterList;
