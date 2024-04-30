#include "../include/spectacularAI/unity/replay.hpp"

#include <iostream>
#include <sstream>
#include <vector>

int main(int argc, char *argv[]) {
    if (argc < 2) {
        std::cout << "Usage: ./main_replay path/to/recording" << std::endl;
        return 1;
    }

    // Recording folder
    std::string dataFolder = argv[1];

    // VIO callback
    callback_t_vio_output onVioOutput = [](const VioOutputWrapper* output) { 
        spectacularAI::Pose pose = sai_vio_output_get_pose(output);
        std::cout << "position = " << pose.position.x << ", " << pose.position.y << ", " << pose.position.z << std::endl;
        sai_vio_output_release(output); // must release memory!
    };

    // SLAM callback
    callback_t_mapper_output onMapperOutput = [](const MapperOutputWrapper* mapperOutput) {
        if (sai_mapper_output_get_final_map(mapperOutput)) {
            std::cout << "SLAM: final map" << std::endl;
        } else {
            std::cout << "SLAM: update map" << std::endl;
        }
        sai_mapper_output_release(mapperOutput); // must release memory!
    };

    spectacularAI::Replay* replayHandle = sai_replay_build(dataFolder.c_str(), "", onMapperOutput);
    sai_replay_set_output_callback(replayHandle, onVioOutput);

    while (sai_replay_one_line(replayHandle));
    sai_replay_release(replayHandle); // must release memory!

    return 0;
}
