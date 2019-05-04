import React, { ReactNode, SyntheticEvent, useState } from "react";
import ContentLoader from "react-content-loader";
import { Carousel } from "react-responsive-carousel";
// tslint:disable-next-line: no-submodule-imports
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { getImageUrl } from "../../services/ApiUtilities";
import { joined } from "../../utilities/Utilities";
import Styles from "./ImageCarousel.module.css";

interface IImageCarousel {
    imageIds?: string[];
    className?: string;
    wrapperClassName: string;
}

// tslint:disable: jsx-no-multiline-js
const ImageCarousel = (props: IImageCarousel) => {
    const [clicked, setClicked] = useState(false);
    const [clickedItem, setClickedItem] = useState(0);

    const getStyle = () => (clicked ? joined(Styles.carousel, Styles.expandedCarousel) : Styles.carousel);
    const onClick = () => setClicked(!clicked);
    const onItemClick = (idx: number) => setClickedItem(idx);

    const className = props.className ? props.className : "";
    return (
        <Carousel
            infiniteLoop={true}
            onChange={onItemClick}
            selectedItem={clickedItem}
            className={joined(getStyle(), className)}
            showStatus={false}
            showThumbs={false}
        >
            {props.imageIds === undefined ? (
                <ImageLoader />
            ) : (
                getCarouselImages(props.imageIds, props.wrapperClassName, clicked, onClick)
            )}
        </Carousel>
    );
};

const setImageToDefault = (evt: SyntheticEvent<HTMLImageElement, Event>) => {
    evt.currentTarget.setAttribute("src", "/placeholder.svg");
    evt.currentTarget.setAttribute(
        "style",
        "height: fit-content;width: 300px;min-width: fit-content;min-height: fit-content;"
    );
};

const getCarouselImages = (
    ids: string[],
    wrapperClassName: string,
    clicked: boolean,
    onClick: () => void
): ReactNode[] => {
    return ids
        .map((id) => getImageUrl(id))
        .map((url, idx) => (
            <div
                x-clicked={clicked.toString()}
                onClick={onClick}
                key={ids[idx]}
                className={Styles.imageWrapper.concat(" ", wrapperClassName)}
            >
                <img
                    alt="Flat preview"
                    x-clicked={clicked.toString()}
                    key={ids[idx]}
                    className={Styles.image}
                    src={url}
                    onError={setImageToDefault}
                />
            </div>
        ));
};

const ImageLoader = () => (
    <ContentLoader
        style={{ width: "100%", height: "100%" }}
        height={100}
        width={100}
        speed={2}
        primaryColor="#f3f3f3"
        secondaryColor="#ecebeb"
        ariaLabel="Kraunasi..."
    >
        <rect x="0" y="0" rx="0" ry="0" width="100" height="100" />
    </ContentLoader>
);

export default ImageCarousel;
