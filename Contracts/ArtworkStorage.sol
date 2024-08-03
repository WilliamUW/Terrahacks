// SPDX-License-Identifier: Unlicensed
pragma solidity ^0.8.19;

contract ArtworkStorage {
    struct Artwork {
        string name;
        string description;
        string prompt;
        string fileUrl;
        int256 x;
        int256 y;
        int256 z;
        uint256 size;
    }

    Artwork[] public artworks;

    constructor() {
        // Initialize artworks in the constructor
        addArtwork(
            "Terracotta Army",
            "The Terracotta Army is a collection of terracotta sculptures depicting the armies of Qin Shi Huang, the first Emperor of China. It is a form of funerary art buried with the emperor in 210-209 BCE with the purpose of protecting the emperor in his afterlife.",
            "Pretend you are the Terracotta Army, an ancient Chinese burial artifact. Share stories about the Qin Dynasty, your purpose, and the history behind your creation.",
            "https://content.mona.gallery/ndpiijit-w1hp-nppi-4kqt-jemzhgjc.glb",
            10,
            5,
            16,
            500
        );

        addArtwork(
            "The Thinker",
            "The Thinker is a bronze sculpture by Auguste Rodin, depicting a man lost in thought. It is often used as an image to represent philosophy.",
            "Pretend you are The Thinker by Auguste Rodin. Share your philosophical thoughts and reflections on life, human nature, and the artist who created you.",
            "https://content.mona.gallery/woa631vy-rsdh-isen-swmi-tfrp2ylu.glb",
            -5,
            0,
            5,
            200
        );

        addArtwork(
            "Eiffel Tower",
            "The Eiffel Tower is a wrought-iron lattice tower on the Champ de Mars in Paris, France. It is named after the engineer Gustave Eiffel, whose company designed and built the tower.",
            "Pretend you are the Eiffel Tower in Paris. Talk about the history of your construction, your architect Gustave Eiffel, and your significance as a global cultural icon.",
            "https://content.mona.gallery/rugswhoy-pujr-dq9e-dvt8-vgc7rmrq.glb",
            -6,
            0,
            -8,
            5
        );

        addArtwork(
            "Starry Night",
            "Starry Night is an oil on canvas painting by Dutch post-impressionist painter Vincent van Gogh. Painted in June 1889, it depicts the view from the east-facing window of his asylum room at Saint-Remy-de-Provence, just before sunrise.",
            "Pretend you are the painting Starry Night by Vincent van Gogh. Describe the emotions and thoughts of Van Gogh as he painted you, and what you represent in the world of art.",
            "https://content.mona.gallery/sprkeiff-bdel-ih9s-1hqh-7unvjh7d.glb",
            -10,
            2,
            -8,
            30
        );

        addArtwork(
            "Christ the Redeemer",
            "Christ the Redeemer is an Art Deco statue of Jesus Christ in Rio de Janeiro, Brazil. Created by French sculptor Paul Landowski and built by Brazilian engineer Heitor da Silva Costa, it is a symbol of Christianity across the world.",
            "Pretend you are Christ the Redeemer in Rio de Janeiro. Speak about your creation, the message of peace and faith you symbolize, and your view over the city.",
            "https://content.mona.gallery/bb9hnnrl-ldpv-5jsb-hurh-xjhzwfqm.glb",
            -5,
            2,
            -7,
            40
        );

        addArtwork(
            "Moai Head",
            "The Moai are monolithic human figures carved by the Rapa Nui people on Easter Island in eastern Polynesia. They were created between 1400 and 1650 CE to honor chieftains or other important people who had passed away.",
            "Pretend you are a Moai head from Easter Island. Share the stories of the Rapa Nui people, the significance of your creation, and the mysteries of your origin.",
            "https://content.mona.gallery/mkv8pl0i-m9pr-qg8n-fg0t-zrf6aijs.glb",
            5,
            1,
            10,
            300
        );

        addArtwork(
            "Mona Lisa",
            "The Mona Lisa is a half-length portrait painting by Italian artist Leonardo da Vinci. It has been described as 'the best known, the most visited, the most written about, the most sung about, the most parodied work of art in the world.'",
            "Pretend you are the Mona Lisa. Talk about Leonardo da Vinci, your enigmatic smile, and your journey through history to become the most famous painting in the world.",
            "https://content.mona.gallery/gfc4yifw-zblg-fayj-osjb-gx0oi3z6.glb",
            5,
            2,
            5,
            100
        );

        addArtwork(
            "Statue of Liberty",
            "The Statue of Liberty is a colossal neoclassical sculpture on Liberty Island in New York Harbor, designed by French sculptor Frederic Auguste Bartholdi and built by Gustave Eiffel. It was a gift from the people of France to the United States.",
            "Pretend you are the Statue of Liberty. Discuss your creation, your symbolism of freedom and democracy, and your significance as a welcoming icon to immigrants arriving by sea.",
            "https://content.mona.gallery/fdypote6-6vvt-tobb-byj5-iktc1r79.glb",
            13,
            -3,
            3,
            10
        );

        addArtwork(
            "American Gothic",
            "American Gothic is a painting by Grant Wood, depicting a farmer standing beside his daughter. The painting is a well-known example of 20th-century American art and is one of the most iconic images in the history of American art.",
            "Pretend you are the painting American Gothic by Grant Wood. Talk about the story behind your creation, the people depicted in the painting, and your place in American cultural history.",
            "https://content.mona.gallery/orasv8bl-vzot-m3sa-hf53-rdm7ba8v.glb",
            2,
            3,
            -4,
            40
        );

        addArtwork(
            "The Scream",
            "The Scream is a composition created by Norwegian artist Edvard Munch. It shows an agonized figure against a tumultuous orange sky and is one of the most recognizable paintings in art history.",
            "Pretend you are the painting The Scream by Edvard Munch. Describe the emotions and psychological states you represent, and the story behind your creation.",
            "https://content.mona.gallery/wd92zwei-kdfr-4cd6-v4jg-4nqsqfhh.glb",
            5,
            2,
            -8,
            200
        );
    }

    function addArtwork(
        string memory _name,
        string memory _description,
        string memory _prompt,
        string memory _fileUrl,
        int256 _x,
        int256 _y,
        int256 _z,
        uint256 _size
    ) public {
        Artwork memory newArtwork = Artwork(
            _name,
            _description,
            _prompt,
            _fileUrl,
            _x,
            _y,
            _z,
            _size
        );
        artworks.push(newArtwork);
    }

    function getArtworkCount() public view returns (uint256) {
        return artworks.length;
    }

    function getArtwork(uint256 _index) public view returns (
        string memory,
        string memory,
        string memory,
        string memory,
        int256,
        int256,
        int256,
        uint256
    ) {
        require(_index < artworks.length, "Invalid artwork index");
        Artwork memory artwork = artworks[_index];
        return (
            artwork.name,
            artwork.description,
            artwork.prompt,
            artwork.fileUrl,
            artwork.x,
            artwork.y,
            artwork.z,
            artwork.size
        );
    }
}
